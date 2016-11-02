#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <WiFiClientSecure.h>
#include "saskey.h"

const char* ssid = "Ralf";
const char* password = "Geheim01!";

const char* mqtt_server = "ButtonIotHub.azure-devices.net";
const char* device_id = "BigButtonDevice";
const char* hub_user = "ButtonIotHub.azure-devices.net/BigButtonDevice";
const char* hub_pwd = SAS_KEY;

const char* redMessage = "{{'color':'Red'}}";
const char* blueMessage = "{{'color':'Blue'}}";



WiFiClientSecure tlsClient;
PubSubClient client(tlsClient);

int redActive=0;
int blueActive=0;

void setup_wifi() {

  delay(10);

  // We start by connecting to a WiFi network
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);

  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
}

void setup(){
  Serial.begin(115200);

  setup_wifi();

  client.setServer(mqtt_server, 8883);

  pinMode(D1, INPUT);               // Set D1 and D2 as input
  pinMode(D2, INPUT);

  pinMode(BUILTIN_LED, OUTPUT);     // Initialize the BUILTIN_LED pin as an output
  digitalWrite(BUILTIN_LED, HIGH);  // Turn the LED off by making the voltage HIGH
}

void reconnect(){
  if(!client.connected()){
    if(client.connect(device_id, hub_user, hub_pwd)){
      Serial.println("Client connected!");
    }
    else{
      Serial.println("Client not connected");
    }
  }
}

void sendMessage(const char* message){
  if(client.connected()){
    Serial.println("Publishing message: " + String(message));
    client.publish("devices/BigButtonDevice/messages/events/", message);
  }
}

void loop(){
  reconnect();
  client.loop();

  int d1 = digitalRead(D1);
  int d2 = digitalRead(D2);

  Serial.println("D1 = "+String(d1)+", D2 = "+String(d2));

  if(!redActive && d1){
    redActive=1;
    sendMessage(redMessage);
  }
  else{
    if(!d1){
      redActive=0;
    }
  }

  if(!blueActive && d2){
    blueActive=1;
    sendMessage(blueMessage);
  }
  else{
    if(!d2){
      blueActive=0;
    }
  }

  if(redActive || blueActive){
    digitalWrite(BUILTIN_LED,LOW); //Led On
  }
  else{
    digitalWrite(BUILTIN_LED, HIGH); //Led Off
  }

  delay(5000);
}
