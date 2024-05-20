#include <ESP8266WiFi.h> 
#include <ESP8266HTTPClient.h> 
#include <WiFiClientSecure.h> 
#include <DHT.h> 
#include <ZMPT101B.h>
#define SENSITIVITY 500.0f
#define DHTTYPE DHT11 //DHT 21 (AM2301)

ZMPT101B voltageSensor(A0, 50.0);

const char* ssid = "Mi Phone"; 
const char* password = "13541357"; 
const char* serverUrl = "https://akushide.ir/IoT-Project/Device/api.php"; // For reading / Changing Status 
const char* dataServerUrl = "https://akushide.ir/IoT-Project/app/v1/api.php"; // Updated URL for the second API 

const int relayPin = D0; // Pin connected to relay 
const int dhtPin = D2; // Pin connected to DHT11 module 
const int analogPin = A0;
int Volt_Amper_Switch = D3; // MUX 



DHT dht(dhtPin, DHTTYPE); 

WiFiClientSecure client; 
HTTPClient http; 

float getVPP()
{
  float result;
  int readValue;         //value read from the sensor
  int maxValue = 0;       // store max value here
  int minValue = 1024;       // store min value here
  
  uint32_t start_time = millis();

  while((millis()-start_time) < 1000) //sample for 1 Sec
  {
      readValue = analogRead(analogPin);
      // see if you have a new maxValue
      if (readValue > maxValue) 
      {
          /*record the maximum sensor value*/
          maxValue = readValue;
      }
      if (readValue < minValue) 
      {
          /*record the maximum sensor value*/
          minValue = readValue;
      }
  }

  // Subtract min from max
  return result = ((maxValue - minValue) * 5)/1024.0;
}


double Current_Function(){
  int mVperAmp = 66; // use 185 for 5A, 100 for 20A Module and 66 for 30A Module
  double AmpsRMS = 0;
  double VRMS = 0;
  double current = 0;
  double Voltage = 0;
  
  Voltage = getVPP();
  VRMS = (Voltage/2.0) *0.53 ; // sq root
  
  AmpsRMS = (VRMS * 1000)/mVperAmp;
  current = AmpsRMS - 0.13;
  if (current <0 ) {current = 0;}
    // Serial.print(current);
    // Serial.println(" current ");
  return current;
}

float Voltage_Function(){
 
 float voltage = voltageSensor.getRmsVoltage();
  // Serial.print("Voltage: ");
  // Serial.println(voltage=1.44*4*voltage/0.7071);

  delay(100); 
  
  return voltage;
}

void setup() 
{ 
    Serial.begin(115200); 
    delay(10); 

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
        
        if (!http.begin(client, serverUrl)) { 
            Serial.println("Failed to connect to server"); 
            return; 
        } 

        int httpCode = http.GET(); 

        if (httpCode > 0) { 
            String payload = http.getString(); 
            Serial.println(payload); 

            if (payload.indexOf("\"Status\":\"on\"") != -1) { // Check the value of "Device" field 
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

        // Make HTTP request to data API 
        if (!http.begin(client, dataServerUrl)) { 
            Serial.println("Failed to connect to data server"); 
            return; 
        } 

        http.addHeader("Content-Type", "application/json"); 

        Volt_Amper_Switch=0; //pin11=0 ==> pin13->out
        float voltage = Voltage_Function();
        delay(200);

        //float voltage = 220.0;

        Volt_Amper_Switch=1;////pin11=1 ==> pin14->out
        double current = Current_Function();
        
        // Send temperature and humidity to the second API 
        float temperature = dht.readTemperature(); 
        float humidity = dht.readHumidity(); 
               
        if (isnan(temperature) || isnan(humidity)) { 
            if(isnan(temperature)) {temperature = 0.0;
            } else {humidity = 0.0;} 
            
            Serial.println("Failed to read from DHT sensor"); 
            //return; 
        } else { 
            String data = "{\"Temp\": " + String(temperature) + ", \"Humidity\": " + String(humidity) + ", \"Current\": " + String(current) + " , \"Voltage\": " + String(voltage) + "}"; 
//            String data = "{\"Temp\": " + String(temperature) + ", \"Humidity\": " + String(humidity) + ", \"Current\": " + String(current) + " , \"Voltage\": " + String(voltage) + "}"; 
                                                           
//            Serial.println(data); 
            
            int dataHttpCode = http.PUT(data); 

            if (dataHttpCode > 0) { 
                Serial.println("Temperature and humidity data sent successfully"); 
            } else { 
                Serial.println("Error sending temperature and humidity data"); 
            } 

            http.end(); 
        } 
    }     
    delay(5000); // Delay for retrieving data again
}