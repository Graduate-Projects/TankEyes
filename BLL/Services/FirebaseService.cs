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
        public static async Task<Models.Client> GetClient(string UserID)
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                var client_info = await firebase.Child("clients").Child(UserID).OnceSingleAsync<BLL.Models.Client>();
                return client_info;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task UpdateClient(string UserID, BLL.Models.Client client)
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                await firebase.Child("clients").Child(UserID).PatchAsync(client);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task UpdateSupplier(string UserID, BLL.Models.Supplier client)
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                await firebase.Child("suppliers").Child(UserID).PatchAsync(client);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<BLL.Models.Client> ChangeConfigPumb(string UserID,bool IsAllowed)
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                await firebase.Child("clients").Child(UserID).Child("auto").PutAsync(IsAllowed.ToString());
                return await GetClient(UserID);
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }
        public static async Task AddNewSupplier(string supplier_id, BLL.Models.Supplier supplier_profile)
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                var supplier = new Dictionary<string, Models.Supplier>();
                supplier.Add(supplier_id, supplier_profile);
                await firebase.Child("suppliers").PatchAsync(supplier);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task AddNewClient(string client_id, Models.Client client_profile)
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                var client = new Dictionary<string, Models.Client>();
                client.Add(client_id, client_profile);
                await firebase.Child("clients").PatchAsync(client);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
