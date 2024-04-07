using System;

namespace LegacyApp
{
    public class UserService
    {
        private IClientRepository _clientRepository;
        private ICreditLimitService _creditLimitService;
        
        public UserService(IClientRepository clientRepository, ICreditLimitService creditLimitService)
        {
            _clientRepository = clientRepository;
            _creditLimitService = creditLimitService;
        }

        [Obsolete]
        public UserService() {}

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!ValidateName(firstName, lastName)) return false;

            if (!ValidateEmail(email)) return false;

            if (!CheckAge(dateOfBirth)) return false;

            var client = _clientRepository.GetById(clientId);

            var user = SetUserData(firstName, lastName, email, dateOfBirth, client);

            SetCreditLimitBasedOnClientType(client, user);

            if (!CheckCreditLimit(user)) return false;

            UserDataAccess.AddUser(user);
            return true;
        }

        private static bool CheckCreditLimit(User user)
        {
            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            return true;
        }

        private void SetCreditLimitBasedOnClientType(Client client, User user)
        {
            if (client.Type == "VeryImportantClient")
            {
                SetCreditLimitForVeryImportantClient(user);
            }
            else if (client.Type == "ImportantClient")
            {
                SetCreditLimitForImportantClient(user);
            }
            else
            {
                SetCreditLimitForNormalClient(user);
            }
        }

        private static void SetCreditLimitForVeryImportantClient(User user)
        {
            user.HasCreditLimit = false;
        }

        private void SetCreditLimitForNormalClient(User user)
        {
            user.HasCreditLimit = true;
            int creditLimit = _creditLimitService.GetCreditLimit(user.LastName, user.DateOfBirth);
            user.CreditLimit = creditLimit;
        }

        private void SetCreditLimitForImportantClient(User user)
        {
            int creditLimit = _creditLimitService.GetCreditLimit(user.LastName, user.DateOfBirth);
            creditLimit = creditLimit * 2;
            user.CreditLimit = creditLimit;
        }

        private static User SetUserData(string firstName, string lastName, string email, DateTime dateOfBirth, Client client)
        {
            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
            return user;
        }

        private static bool CheckAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            if (age < 21)
            {
                return false;
            }

            return true;
        }

        private static bool ValidateEmail(string email)
        {
            if (!email.Contains("@") && !email.Contains("."))
            {
                return false;
            }

            return true;
        }

        private static bool ValidateName(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }

            return true;
        }
    }
}
