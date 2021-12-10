using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public static class FirebaseService
    {
        public static async Task<Models.Client> UpdateInfoTank(Guid UserID)
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                var client_info = await firebase.Child("clients").Child(UserID.ToString()).OnceSingleAsync<BLL.Models.Client>();
                return client_info;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static async Task<BLL.Models.Client> TriggerPumb(Guid UserID)
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                var client_info = await firebase.Child("clients").Child(UserID.ToString()).OnceSingleAsync<BLL.Models.Client>();
                client_info.status = !client_info.status;
                await firebase.Child("clients").Child(UserID.ToString()).PutAsync(Newtonsoft.Json.JsonConvert.SerializeObject(client_info));
                return client_info;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static async Task<BLL.Models.Client> ChangeConfigPumb(Guid UserID,bool IsAllowed)
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                var client_info = await firebase.Child("clients").Child(UserID.ToString()).OnceSingleAsync<BLL.Models.Client>();
                client_info.auto = IsAllowed;
                await firebase.Child("clients").Child(UserID.ToString()).PutAsync(Newtonsoft.Json.JsonConvert.SerializeObject(client_info));
                return client_info;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static async Task<IEnumerable<BLL.Models.Supplier>> GetAllSuppliersAsync()
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                var values = await firebase.Child("suppliers").OnceSingleAsync<Dictionary<string, BLL.Models.Supplier>>();
                var suppliers = values.Select(item => item.Value)?.ToList();
                return suppliers;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static async Task AddNewSupplier(BLL.Models.Supplier supplier)
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                await firebase.Child("suppliers").PostAsync(supplier);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
