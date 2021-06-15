using Android.App;
using Android.OS;
using AndroidX.AppCompat.App;
using Android.Graphics;
using Android.Widget;
using Plugin.Media;
using System;
using System.IO;
using Plugin.CurrentActivity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Android.Runtime;
using Android.Content;
using Android.Views;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AzureListaClientes
{
    [Activity(Label = "Agregar")]
    public class Agregar : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Agregar);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            var btnAlmacenar = FindViewById<Button>(Resource.Id.btnguardar);
            var txtNombre = FindViewById<EditText>(Resource.Id.txtnombre);
            var txtCorreo = FindViewById<EditText>(Resource.Id.txtemail);
            var txtDomicilio = FindViewById<EditText>(Resource.Id.txtdireccion);
            var txtEdad = FindViewById<EditText>(Resource.Id.txtedad);
            var txtEscuela = FindViewById<EditText>(Resource.Id.txtescuela);
            var txtjob = FindViewById<EditText>(Resource.Id.txtjob);
            var imagen = FindViewById<ImageView>(Resource.Id.imagen);
            imagen.SetImageResource(Resource.Drawable.LaSalleLogo);


            btnAlmacenar.Click += async delegate
            {
                try
                {
                    var CuentadeAlmacenamiento = CloudStorageAccount.Parse
                    ("DefaultEndpointsProtocol=https;AccountName=storagealvaro;AccountKey=NCqB2yRC6cKyN9MPMxCBU5eDdjilWu3IS/RaiWM+fENbtlbtEDXEzddtbTJFHDibF54jiy5+EkGyL2Wc3qL4RQ==;BlobEndpoint=https://storagealvaro.blob.core.windows.net/;QueueEndpoint=https://storagealvaro.queue.core.windows.net/;TableEndpoint=https://storagealvaro.table.core.windows.net/;FileEndpoint=https://storagealvaro.file.core.windows.net/;");
                    var ClienteBlob = CuentadeAlmacenamiento.CreateCloudBlobClient();
                    var Carpeta = ClienteBlob.GetContainerReference("alvaro");
                    var TablaNoSQL = CuentadeAlmacenamiento.CreateCloudTableClient();
                    var Coleccion = TablaNoSQL.GetTableReference("devs");
                    await Coleccion.CreateIfNotExistsAsync();
                    var clientes = new Clientes("Clientes", txtNombre.Text);
                    clientes.Correo = txtCorreo.Text;
                    clientes.Domicilio = txtDomicilio.Text;
                    clientes.Edad = int.Parse(txtEdad.Text);
                    clientes.Imagen = "ocho.jpg";
                    clientes.ImagenFondo = "ochofondo.jpg";
                    clientes.Latitud = 150.6517879;
                    clientes.Longitud = -74.0267784;                    
                    clientes.Ocupación = txtjob.Text;
                    clientes.Escuela = txtEscuela.Text;
                    var Store = TableOperation.Insert(clientes);
                    await Coleccion.ExecuteAsync(Store);
                    Toast.MakeText(this, "Datos Guardados en Tabla NoSQL en Azure", ToastLength.Long).Show();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };

            var txtVolver = FindViewById<TextView>(Resource.Id.txtvolver);

            txtVolver.Click += delegate
            {
                var Volver = new Intent(this, typeof(app1));
                StartActivity(Volver);
            };
            // Create your application here
        }
        public class Clientes : TableEntity
        {
            public Clientes(string Categoria, string Nombre)
            {
                PartitionKey = Categoria;
                RowKey = Nombre;
            }
            public string Correo { get; set; }
            public string Domicilio { get; set; }
            public int Edad { get; set; }
            public string Imagen { get; set; }
            public string ImagenFondo { get; set; }
            public double Latitud { get; set; } 
            public double Longitud { get; set; }
            public string Ocupación { get; set; }
            public string Escuela { get; set; }
        }
    }    
}