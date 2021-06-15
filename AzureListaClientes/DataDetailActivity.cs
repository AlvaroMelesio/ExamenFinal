using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using AndroidX.Core.Graphics.Drawable;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;


namespace AzureListaClientes
{
    [Activity(Label = "DataDetailActivity")]
    public class DataDetailActivity : Activity, Android.Gms.Maps.IOnMapReadyCallback
    {
        TextView txtNombre, txtDomicilio, txtCorreo, txtEdad, txtOcupacion, txtEscuela;
        ImageView Imagen, ImagenFondo;
        string correo, imagen, nombre, domicilio, imagenfondo, ocupacion, escuela;
        double lat, lon;
        int edad;
        GoogleMap googleMap;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DataDetail);
            try
            {
                correo = Intent.GetStringExtra("correo");
                imagen = Intent.GetStringExtra("imagen");
                imagenfondo = Intent.GetStringExtra("imagenfondo");
                nombre = Intent.GetStringExtra("nombre");
                edad = int.Parse(Intent.GetStringExtra("edad"));
                lat = double.Parse(Intent.GetStringExtra("latitud"));
                lon = double.Parse(Intent.GetStringExtra("longitud"));
                domicilio = Intent.GetStringExtra("domicilio");
                ocupacion = Intent.GetStringExtra("Ocupación");
                escuela = Intent.GetStringExtra("Escuela");

                ImagenFondo = FindViewById<ImageView>(Resource.Id.imagenfondo);
                Imagen = FindViewById<ImageView>(Resource.Id.imagen);
                txtNombre = FindViewById<TextView>(Resource.Id.txtnombre);
                txtDomicilio = FindViewById<TextView>(Resource.Id.txtaddress);
                txtCorreo = FindViewById<TextView>(Resource.Id.txtmail);
                txtEdad = FindViewById<TextView>(Resource.Id.txtedad);
                txtOcupacion = FindViewById<TextView>(Resource.Id.txtjob);
                txtEscuela = FindViewById<TextView>(Resource.Id.txtEscuela);

                txtNombre.Text = nombre;
                txtDomicilio.Text = domicilio;
                txtCorreo.Text = correo;
                txtEdad.Text = edad.ToString();
                txtOcupacion.Text = ocupacion;
                txtEscuela.Text = escuela;

                var typeface = Typeface.CreateFromAsset(this.Assets, "Fonts/OdibeeSans-Regular.ttf");
                txtNombre.SetTypeface(typeface, TypefaceStyle.Normal);
                var RutaImagen = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), imagen);
                var RutaImagenFondo = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), imagenfondo);
                var rutauriimagen = Android.Net.Uri.Parse(RutaImagen);
                var rutauriimagenfondo = Android.Net.Uri.Parse(RutaImagenFondo);
                Imagen.SetImageURI(rutauriimagen);
                ImagenFondo.SetImageURI(rutauriimagenfondo);

                var opciones = new BitmapFactory.Options();
                opciones.InPreferredConfig = Bitmap.Config.Argb8888;
                var bitmap = BitmapFactory.DecodeFile(RutaImagen, opciones);
                Imagen.SetImageDrawable(getRoundedCornerImage(bitmap, 20));

                var mapView = FindViewById<MapView>(Resource.Id.mapa);
                mapView.OnCreate(savedInstanceState);
                mapView.GetMapAsync(this);
                MapsInitializer.Initialize(this);
            }
            catch (Exception)
            {

                throw;
            }
            // Create your application here
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            this.googleMap = googleMap;
            var builder = CameraPosition.InvokeBuilder();
            builder.Target(new LatLng(lat, lon));
            builder.Zoom(17);
            var cameraPosition = builder.Build();
            var cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            this.googleMap.AnimateCamera(cameraUpdate);
        }

        public static Android.Support.V4.Graphics.Drawable.RoundedBitmapDrawable getRoundedCornerImage(Bitmap image, int cornerRadius)
        {
            var corner = Android.Support.V4.Graphics.Drawable.RoundedBitmapDrawableFactory.Create(null, image);
            corner.CornerRadius = cornerRadius;
            return corner;
        }
    }
}