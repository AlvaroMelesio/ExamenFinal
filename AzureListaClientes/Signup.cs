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
    [Activity(Label = "Signup")]
    public class Signup : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.signup);

            var txtUsuario = FindViewById<EditText>(Resource.Id.txtusuario);
            var txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            var btnSignup = FindViewById<Button>(Resource.Id.btnsignup);
            var txtVolver = FindViewById<TextView>(Resource.Id.txtvolver);
            var Prueba = FindViewById<TextView>(Resource.Id.textView2);
            var WS = new Azure.WebService();
            int test = 0;

            btnSignup.Click += delegate
            {
                var Conjunto = new DataSet();
                DataRow Renglon;
                try
                {
                    Conjunto = WS.NoSignUp_ExF(txtUsuario.Text);
                    Renglon = Conjunto.Tables["NoSignUp"].Rows[0];
                    test = (int)Renglon["NumAccounts"];
                    if (test == 0)
                    {
                        try
                        {
                            if (WS.SignUp_ExF(txtUsuario.Text, txtPassword.Text))
                            {
                                Toast.MakeText(this, "Registrado Correctamente", ToastLength.Long).Show();
                                var Login = new Intent(this, typeof(Login));
                                StartActivity(Login);
                            }
                            else
                                Toast.MakeText(this, "No Registrado Correctamente", ToastLength.Long).Show();
                        }
                        catch (System.Exception ex)
                        {
                            Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(this, "El Usuario Ya Existe", ToastLength.Long).Show();
                    }
                }
                catch (System.Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
                
                txtVolver.Click += delegate
                {
                    var Volver = new Intent(this, typeof(Login));
                    StartActivity(Volver);
                };
                // Create your application here
            };
        }
    }
}