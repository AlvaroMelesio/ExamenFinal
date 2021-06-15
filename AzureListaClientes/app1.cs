using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzureListaClientes
{
    [Activity(Label = "app1")]
    public class app1 : Activity
    {
        string Usuario;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.app1);

            var txtBienvenido = FindViewById<TextView>(Resource.Id.txtbienvenido);
            var txtSalir = FindViewById<TextView>(Resource.Id.txtsalir);
            var btnGuardar = FindViewById<TextView>(Resource.Id.btnguardar);
            var btnBuscar = FindViewById<TextView>(Resource.Id.btnbuscar);
            var Imagen = FindViewById<ImageView>(Resource.Id.imagen);

            Usuario = Intent.GetStringExtra("Usuario");
            txtBienvenido.Text = "Bienvenido " + Usuario + ".";
            Imagen.SetImageResource(Resource.Drawable.LaSalleLogo);

            txtSalir.Click += delegate
            {
                System.Environment.Exit(0);
            };

            btnGuardar.Click += delegate
            {
                var Buscar = new Intent(this, typeof(Agregar));
                Buscar.PutExtra("Usuario", Usuario);
                StartActivity(Buscar);
            };

            btnBuscar.Click += delegate
            {
                var Calcular = new Intent(this, typeof(MainActivity));
                Calcular.PutExtra("Usuario", Usuario);
                StartActivity(Calcular);
            };
        }
    }
}