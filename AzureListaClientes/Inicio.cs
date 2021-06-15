using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;
using System.Linq;
using Android.Content;
using System.Threading.Tasks;
using Android.Graphics;

namespace AzureListaClientes
{
    [Activity(Label = "Inicio", MainLauncher = true)]
    public class Inicio : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Inicio);

            var Imagen = FindViewById<ImageView>(Resource.Id.imagen);
            Imagen.SetImageResource(Resource.Drawable.LaSalleLogo);            

            Imagen.Click += delegate
            {
                var Siguiente = new Intent(this, typeof(Login));
                StartActivity(Siguiente);
            };
            // Create your application here

        }
    }
}