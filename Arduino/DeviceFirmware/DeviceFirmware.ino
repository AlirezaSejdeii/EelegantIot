#include <ESP8266WiFi.h> 
#include <ESP8266HTTPClient.h> 
#include <WiFiClientSecure.h> 
#include <DHT.h> 
#include <ZMPT101B.h>
#include <Adafruit_ADS1X15.h>
#include <ArduinoJson.h>
Adafruit_ADS1115 ads;  /* Use this for the 16-bit version */

#define SENSITIVITY 500.0f

#define DHTPIN D7            //what pin we're connected to
#define DHTTYPE DHT21       //DHT 21  (AM2301)
DHT dht(DHTPIN, DHTTYPE);   //Initialize DHT sensor for normal 16mhz Arduino

#define ADS1115_ADDRESS 0x48  // I2C address of ADS1115


ZMPT101B voltageSensor(A0, 50.0);

// const char* ssid = "MobileInternet"; //"Mi Phone"; 
// const char* password = "13541357"; 
const char* ssid = "MobinNet_2472";
const char* password = "8311100@alireza!";
const char* serverUrl = "https://elegant.liara.run";
const char* identifier = "iot-device-0001";

const int relayPin = D0; // Pin connected to relay 
const int dhtPin = D5; // Pin connected to DHT11 module 
const float ScaleFactor = 863*0.1875/1000.0;
const float IScaleFactor = 4.95/65.536;
const float VoltageOffset = 2.0;
const float CurrentOffset = 1.5;



//#define ACS_Pin A0                        //Sensor data pin on A0 analog   input
const int numReadings = 100;  // Reduced number of readings for faster computation
int voltageReadings[numReadings];  // Array to store voltage readings
int currentReadings[numReadings];  // Array to store current readings

int currentIndex = 0;         // Index for the readings array
int voltageIndex = 0;         // Index for the readings array

int vIndex = 100;          
int iIndex = 100;

int totalVoltage = 0;   // Total sum of the voltage readings
int totalCurrent = 0;   // Total sum of the current readingsfloat Irms = 0.0;              // Root mean square value

float sensitivity = 66;    // Sensitivity of ACS712 sensor (mV per A) for the 30A version
float offset = 2500;          // Offset voltage of ACS712 sensor (mV)

float humidity;  //Stores humidity value
float temperature; //Stores temperature value

int16_t adc0, adc1;
float voltage, current;

// Scaling factor to convert analog readings to AC voltage
float voltagePerAnalogUnit = 1; //240.0 / 1023.0;

WiFiClientSecure client; 
HTTPClient http; 

void setup() 
{ 
    Serial.begin(115200); 
    delay(10); 
    
    dht.begin();
   
    if (!ads.begin())
    {
     Serial.println("Failed to initialize ADS.");
     while (1);
    }

    //pinMode(ACS_Pin,INPUT);  //Define the pin mode

    pinMode(relayPin, OUTPUT); 
    digitalWrite(relayPin, LOW); 

    WiFi.begin(ssid, password); 
    Serial.print("Connecting to WiFi"); 

    while (WiFi.status() != WL_CONNECTED) { 
        delay(1000); 
        Serial.print("."); 
    } 

    Serial.println(); 
    Serial.print("Connected to WiFi, IP address: "); 
    Serial.println(WiFi.localIP()); 
    client.setInsecure(); 
} 

void loop() 
{ 
    if (WiFi.status() == WL_CONNECTED) { 
        
        if (!http.begin(client, String(serverUrl) + "/device-manager/rely-status/" + identifier)) {
           Serial.println("Failed to connect to server");
           return;
        }

        int httpCode = http.GET(); 

        if (httpCode > 0) { 
          String payload = http.getString();
          Serial.println(payload);
          DynamicJsonDocument docToReadData(1024);  // Create a JSON document object
          deserializeJson(docToReadData, payload);
    
          if (docToReadData["on"]) { // Check the value of "Device" field 
                digitalWrite(relayPin, HIGH); // Turn on the relay 
                Serial.println("Relay turned on"); 
            } else { 
                digitalWrite(relayPin, LOW); // Turn off the relay 
                Serial.println("Relay turned off"); 
            } 
        } else { 
            Serial.println("Error in HTTP request"); 
        } 

        http.end(); 

///////////////////////////////////////////////////////
/// Voltage and Current Read Process
//////////////////////////////////////////////////////
        // Make HTTP request to data API 
        if (!http.begin(client, String(serverUrl) + "/device-manager/update-values/" + identifier)) { 
            Serial.println("Failed to connect to data server"); 
            return; 
        } 

        http.addHeader("Content-Type", "application/json"); 

        for (int i = 0; i < numReadings; i++) {
          // Read analog voltage from the ZMPT101B module via the multiplexer
            voltageReadings[i] = ads.readADC_SingleEnded(0);
            currentReadings[i] = ads.readADC_SingleEnded(1);
            delay(10);  // Adjust delay for sampling rate
         }

          // Calculate and print RMS voltage
         float rmsVoltage = calculateRMS(voltageReadings, numReadings, ScaleFactor); // Scaling factor: 0.1875 / 1000.0
         rmsVoltage = rmsVoltage - VoltageOffset;
         if (rmsVoltage < 0) {rmsVoltage=0;}

         Serial.print("RMS Voltage: ");
         Serial.print(rmsVoltage);
         Serial.println(" V");
  
        float rmsCurrent = calculateRMS(currentReadings, numReadings, IScaleFactor); // Scaling factor: 0.1875 / 1000.0      
         rmsCurrent = rmsCurrent - CurrentOffset;
         if (rmsCurrent < 0) {rmsCurrent=0;}
                   
          Serial.print("RMS Current: ");
          Serial.print(rmsCurrent);
          Serial.println(" A");
  

  
///////////////////////////////////////////////////////
/// Temp and Hum. Reading Process
//////////////////////////////////////////////////////

        delay(200);

        // Send temperature and humidity to the second API 
        temperature = dht.readTemperature();
        humidity = dht.readHumidity();

///////////////////////////////////////////////////////
/// Sending Data to Server Process
//////////////////////////////////////////////////////
              
        if (isnan(temperature) || isnan(humidity)) { 
            if(isnan(temperature)) {temperature = 0.0;
            } else {humidity = 0.0;} 
            
            Serial.println("Failed to read from DHT sensor"); 
            return; 
        } else { 
            StaticJsonDocument<1024> docToSendData;
            docToSendData["temperature"] = temperature;
            docToSendData["humidity"] = humidity;
            docToSendData["current"] = rmsCurrent;
            docToSendData["voltage"] = rmsVoltage;

            String jsonData;
            serializeJson(docToSendData, jsonData);
            Serial.println(jsonData);
            int dataHttpCode = http.PUT(jsonData); 

            if (dataHttpCode > 0) { 
                //Serial.println("Temperature and humidity data sent successfully"); 
            } else { 
                Serial.println("Error sending temperature and humidity data"); 
            } 

            http.end(); 
        } 
    }

    delay(3000); // Delay for retrieving data again

}

float calculateRMS(int readings[], int numReadings, float scaleFactor) {
  
  int sum=0;
  for (int i = 0; i < numReadings; ++i) {
    sum  = sum + readings[i];}
  
  float Imean = sum / (float)numReadings; // Calculate mean of the readings
  float IsquaredSum = 0.0; // Sum of squared differences from the mean
  
  // Calculate sum of squared differences from the mean
  for (int i = 0; i < numReadings; ++i) {
    float Idiff = readings[i] - Imean;
    IsquaredSum += Idiff * Idiff;
  }
  
  // Calculate RMS value
  float IVrms = sqrt(IsquaredSum / numReadings) * scaleFactor;
  return IVrms;
}