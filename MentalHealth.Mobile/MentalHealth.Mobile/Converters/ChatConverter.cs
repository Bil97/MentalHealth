using MentalHealth.Models;
using MentalHealth.Models.UserAccount;
using MentalHealth.Services;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MentalHealth.Mobile.Converters
{
    public class ImageUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imageUrl = $"{BaseApi.Url}api/ApplicationUsers/image/img_avatar.jpg";
            if (!string.IsNullOrEmpty(value?.ToString()))
                imageUrl = $"{BaseApi.Url}api/ApplicationUser/image/{value}";

            return imageUrl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ChatUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "";

            switch (value)
            {
                case Chat message:
                    {
                        string fullname;
                        string userId;
                        string imageUrl = $"{BaseApi.Url}api/ApplicationUsers/image/img_avatar.jpg";
                        string username;
                        LayoutOptions position;
                        string background;

                        if (message.CurrentUserId != message.SenderAId)
                        {
                            username = fullname = message.SenderA.FullName;
                            userId = message.SenderA.Id;
                            if (!string.IsNullOrEmpty(message.SenderA.ImagePath))
                                imageUrl = $"{BaseApi.Url}api/ApplicationUser/image/{message.SenderA.ImagePath}";
                        }
                        else
                        {
                            username = fullname = message.SenderB.FullName;
                            userId = message.SenderB.Id;
                            if (!string.IsNullOrEmpty(message.SenderB.ImagePath))
                                imageUrl = $"{BaseApi.Url}api/ApplicationUser/image/{message.SenderB.ImagePath}";
                        }

                        if (message.CurrentUserId == message.SenderId)
                        {
                            username = message.Sender?.FullName;
                            position = LayoutOptions.EndAndExpand;
                            background = "Gray";
                        }
                        else
                        {
                            position = LayoutOptions.StartAndExpand;
                            background = "Aquamarine";
                        }

                        switch (parameter.ToString())
                        {
                            case "fullname":
                                result = fullname;
                                break;
                            case "userId":
                                result = userId;
                                break;
                            case "imageUrl":
                                result = imageUrl;
                                break;
                            case "username":
                                result = username;
                                break;
                            case "position":
                                return position;
                            case "background":
                                result = background;
                                break;
                        }

                        break;
                    }
                case UserProfession profession:
                    {
                        if (parameter?.ToString() == "therapist")
                        {
                            var amount = profession.ServiceFee.ToString("C2", CultureInfo.CreateSpecificCulture("sw-KE"));
                            result = $"Consult {{ {amount} }}";
                        }
                        else
                        {
                            var authToken = Application.Current.Properties["authToken"]?.ToString();
                            var user = App.User.AuthenticationState(authToken);

                            if (profession.User?.UserName != user.Identity.Name && user.IsInRole("User"))
                                result = "true";
                            else
                                result = "false";
                        }
                        break;
                    }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class PeriodConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "";
            var authToken = Application.Current.Properties["authToken"]?.ToString();

            if (value is SessionRecord session)
            {
                switch (parameter.ToString())
                {
                    case "period":
                        {
                            var format1 = string.Format("{0:dd/MM/yyyy HH:mm:ss}", session.DateStarted);

                            if (session.IsComplete)
                            {
                                var format2 = string.Format("{0:dd/MM/yyyy HH:mm:ss}", session.DateEnded);
                                result = $"Period: {format1} to {format2}";
                            }
                            else
                            {
                                result = $"Period: {format1} to present";
                            }
                            break;
                        }
                    case "healthOfficer":
                        result = session.Patient?.FullName;
                        break;
                    case "user":
                        result = session.HealthOfficer?.FullName;
                        break;
                    case "amount":
                        result = session.Amount.ToString("C2", CultureInfo.CreateSpecificCulture("sw-KE"));
                        break;
                    case "patientvisibility":
                        if (App.User.AuthenticationState(authToken).IsInRole("HealthOfficer"))
                            result = "false";
                        else result = "true";
                        break;
                      case "healthOfficervisibility":
                      if (App.User.AuthenticationState(authToken).IsInRole("User"))
                            result = "false";
                        else result = "true";
                        break;
                }
            }
            else if (value is PatientHealthRecord record)
            {
                switch (parameter.ToString())
                {
                    case "date":
                        result = string.Format("{0:dd/MM/yyyy HH:mm:ss}", record.Date);
                        break;
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
