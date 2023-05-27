// Store a global phone number as a string
using PhoneNumbers;

string phoneNumber = "1066643039";

// Create a PhoneNumberUtil instance
PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();

// Parse the phone number
PhoneNumber parsedPhoneNumber = phoneNumberUtil.Parse(phoneNumber, "EG");

// Validate the phone number
bool isValidPhoneNumber = phoneNumberUtil.IsValidNumber(parsedPhoneNumber);

// Format the phone number
string formattedPhoneNumber = phoneNumberUtil.Format(parsedPhoneNumber, PhoneNumberFormat.INTERNATIONAL);

Console.WriteLine(formattedPhoneNumber);