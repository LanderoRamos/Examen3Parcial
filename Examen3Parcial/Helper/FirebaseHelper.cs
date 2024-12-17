using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

namespace Examen3Parcial.Helper
{
    public class FirebaseHelper
    {

        private readonly FirebaseClient firebaseClient;

        public FirebaseHelper()
        {
            firebaseClient = new FirebaseClient("https://tarea31-fce36-default-rtdb.firebaseio.com/");
        }

        public async Task<List<Modelos.Notas>> GetAllProducto()
        {
            var produto = await firebaseClient
                .Child("NOTAS")
                .OnceAsync<Modelos.Notas>();

            return produto.Select(item => new Modelos.Notas
            {
                ID_NOTA = item.Key,
                DESCRIPCION = item.Object.DESCRIPCION,
                FECHA = item.Object.FECHA,
                PHOTO_RECORD = item.Object.PHOTO_RECORD,
                FECHAINGRESO = item.Object.FECHAINGRESO,
               
            })
                .OrderByDescending(nota => DateTime.Parse(nota.FECHAINGRESO))
                .ToList();
        }

        public async Task AddProducto(Modelos.Notas notas)
        {
            await firebaseClient
                .Child("NOTAS")
                .PostAsync(notas);
        }

        public async Task UpdateProducto(String key, Modelos.Notas notas)
        {
            await firebaseClient
                .Child("NOTAS")
                .Child(key)
                .PutAsync(notas);
        }

        public async Task DeleteProducto(String key)
        {
            await firebaseClient
                .Child("NOTAS")
                .Child(key)
                .DeleteAsync();
        }

    }
}
