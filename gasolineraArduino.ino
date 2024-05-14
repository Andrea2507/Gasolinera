#include <ArduinoJson.h>

const int led = 13;

void setup() {
  Serial.begin(9600);
  pinMode(led, OUTPUT);
}

void loop() {
  if (Serial.available() > 0) {
 
    String jsonData = Serial.readStringUntil('\n');

    
    StaticJsonDocument<64> jsonDocument;
    
    
    DeserializationError error = deserializeJson(jsonDocument, jsonData);

   

    // Obtener el valor de la propiedad 'action'
    String action = jsonDocument["action"];

    // Realizar acciones basadas en el valor de 'action'
    if (action == "Encender") {
      digitalWrite(led, HIGH);
    } 
    else if (action == "Apagar") {
      digitalWrite(led, LOW);
    } else {
      // Acción no reconocida
    }
  }
}
  