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

        public static async Task<IEnumerable<string>> GetRegionsAsync()
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                var Countries = await firebase.Child("countries").OnceAsync<Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>>();
                return Countries
                        .Select(item => item.Object)
                            .SelectMany(item => item.Values)
                                .SelectMany(item => item.Values)
                                    .SelectMany(item => item.Keys);
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
        public static async Task AddNewOrder(string order_id, Models.Order order_details)
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                var order = new Dictionary<string, Models.Order>();
                order.Add(order_id, order_details);
                await firebase.Child("orders").PatchAsync(order);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<IEnumerable<BLL.Models.Order>> GetAllOrdersAsync()
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                var values = await firebase.Child("orders").OnceSingleAsync<Dictionary<string, BLL.Models.Order>>();
                var orders = values.Select(item => item.Value)?.ToList();
                return orders;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<IEnumerable<BLL.Models.Order>> GetOrdersClientsAsync(string client_id)
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                var values = await firebase.Child("orders").OnceSingleAsync<Dictionary<string, BLL.Models.Order>>();
                var orders = values.Select(item => item.Value)?.ToList();
                return orders.Where(item=>item.client_id == client_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<IEnumerable<BLL.Models.Order>> GetOrdersSupplierAsync(string supplier_id)
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                var values = await firebase.Child("orders").OnceSingleAsync<Dictionary<string, BLL.Models.Order>>();
                var orders = values.Select(item => item.Value)?.ToList();
                return orders.Where(item => item.supplier_id == supplier_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task UpdateOrder(string order_id, BLL.Models.Order order_details)
        {
            try
            {
                var firebase = new FirebaseClient(BLL.Configration.FirebaseConfigration.DatabaseURL);
                await firebase.Child("orders").Child(order_id).PatchAsync(order_details);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
