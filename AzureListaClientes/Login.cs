using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AzureListaClientes
{
    [Activity(Label = "Login")]
    public class Login : Activity
    {
        string Usuario;
        EditText txtUsuario;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.login);
            // Create your application here

            var btnIngresar = FindViewById<Button>(Resource.Id.btningresar);
            txtUsuario = FindViewById<EditText>(Resource.Id.txtusuario);
            var Imagen = FindViewById<ImageView>(Resource.Id.imagen);
            var txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            var btnSignup = FindViewById<TextView>(Resource.Id.txtrsignup);
            Imagen.SetImageResource(Resource.Drawable.LaSalleLogo);

            Usuario = txtUsuario.Text;


            txtUsuario.Click += delegate
            {
                txtUsuario.Text = null;
            };

            txtPassword.Click += delegate
            {
                txtPassword.Text = null;
            };

            btnIngresar.Click += delegate
            {
                var WS = new Azure.WebService();
                int test = 0;

                var Conjunto = new DataSet();
                DataRow Renglon;

                try
                {
                    Conjunto = WS.CountLogin_ExF(txtUsuario.Text, txtPassword.Text);
                    Renglon = Conjunto.Tables["Login"].Rows[0];
                    test = (int)Renglon["NumAccounts"];
                    if (test == 0)
                    {
                        Toast.MakeText(this, "Las Credenciales Son Incorrextas", ToastLength.Long).Show();
                    }
                    else
                    {
                        Cargar();
                    }
                }
                catch (System.Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };

            btnSignup.Click += delegate
            {
                var Registro = new Intent(this, typeof(Signup));
                StartActivity(Registro);
            };
        }

        public void Cargar()
        {
            var Siguiente = new Intent(this, typeof(app1));
            Siguiente.PutExtra(txtUsuario.Text, Usuario);
            StartActivity(Siguiente);
        }
    }
}