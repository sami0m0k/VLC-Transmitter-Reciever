void setup()
{
	Serial.begin(9600);
	Serial3.begin(9600);
}

void loop()
{
	if (Serial3.available())
	{
		char data = Serial3.read();
		Serial.write(data);
	}
}