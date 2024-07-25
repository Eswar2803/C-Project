using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Web;

namespace WindowsFormsApp1.UI
{
    public partial class frmOTPmsg : Form
    {
        string randomNumber;

        public frmOTPmsg()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string apiKey = "NmY0Mjc2MzQ3NzcyNzc2NTZmNmQ2OTUyNGU1NTc2Nzk=";
                string phoneNumber = txtPhone.Text.Trim(); // Phone number input

                // Validate phone number format
                if (!IsValidIndianPhoneNumber(phoneNumber))
                {
                    MessageBox.Show("Please enter a valid Indian phone number.");
                    return;
                }

                string smsSender = "Annaa Silicon Technology Pvt. Ltd"; // Changed variable name from 'sender' to 'smsSender'
                string name = txtName.Text;
                Random rnd = new Random();
                randomNumber = rnd.Next(100000, 999999).ToString();
                string message = $"Hey {name}, Your OTP is {randomNumber}";

                // URL encode parameters
                string urlEncodedMessage = HttpUtility.UrlEncode(message);
                string urlEncodedSender = HttpUtility.UrlEncode(smsSender);
                string urlEncodedPhoneNumber = HttpUtility.UrlEncode(phoneNumber);

                string url = $"https://api.txtlocal.com/send/?apikey={apiKey}&numbers={urlEncodedPhoneNumber}&message={urlEncodedMessage}&sender={urlEncodedSender}";

                // Create HTTP request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET"; // Use GET method for this API

                // Get response
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    // Check the result to see if the SMS was sent successfully
                    MessageBox.Show("OTP Sent Successfully. Response: " + result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (txtVerifyOTP.Text == randomNumber)
            {
                MessageBox.Show("Logged In Successfully");
            }
            else
            {
                MessageBox.Show("Wrong OTP");
            }
        }

        private bool IsValidIndianPhoneNumber(string number)
        {
            // Indian phone numbers should be 10 digits long and optionally start with +91 or 91
            return System.Text.RegularExpressions.Regex.IsMatch(number, @"^(\+91|91)?\d{10}$");
        }
    }
}
