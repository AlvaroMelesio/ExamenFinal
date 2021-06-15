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
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        Android.App.ProgressDialog progress;
        string elementoimagen, elementoimagenfondo;
        ListView listado;
        List<Clientes> ListadeClientes = new List<Clientes>();
        List<ElementosdelaTabla> ElementosTabla = new List<ElementosdelaTabla>();
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            SupportActionBar.Hide();
            listado = FindViewById<ListView>(Resource.Id.lista);
            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Cargando datos de Azure...");
            progress.SetCancelable(false);
            progress.Show();
            await CargarDatosAzure();
            progress.Hide();
        }

        public async Task CargarDatosAzure()
        {
            try
            {
                var typeface = Typeface.CreateFromAsset(this.Assets, "Fonts/OdibeeSans-Regular.ttf");
                var Titulo = FindViewById<TextView>(Resource.Id.txttitulo);
                Titulo.SetTypeface(typeface, TypefaceStyle.Normal);
                var CuentaAlmacenamiento = CloudStorageAccount.Parse
                    ("DefaultEndpointsProtocol=https;AccountName=storagealvaro;AccountKey=NCqB2yRC6cKyN9MPMxCBU5eDdjilWu3IS/RaiWM+fENbtlbtEDXEzddtbTJFHDibF54jiy5+EkGyL2Wc3qL4RQ==;EndpointSuffix=core.windows.net");
                var ClienteBlob = CuentaAlmacenamiento.CreateCloudBlobClient();
                var Contenedor = ClienteBlob.GetContainerReference("alvaro");
                var TablaNoSQL = CuentaAlmacenamiento.CreateCloudTableClient();
                var Tabla = TablaNoSQL.GetTableReference("devs");
                var Consulta = new TableQuery<Clientes>();
                TableContinuationToken token = null;
                var Datos = await Tabla.ExecuteQuerySegmentedAsync<Clientes>
                    (Consulta, token, null, null);
                ListadeClientes.AddRange(Datos.Results);
                int iCorreo = 0;
                int iNombre = 0;
                int iImagen = 0;
                int iEdad = 0;
                int iDomicilio = 0;
                int iSaldo = 0;
                int iLatitud = 0;
                int iLongitud = 0;
                int iImagenFondo = 0;
                int iOcupacion = 0;
                int iEscuela = 0;
                ElementosTabla = ListadeClientes.Select(r => new ElementosdelaTabla()
                {
                    Correo = ListadeClientes.ElementAt(iCorreo++).Correo,
                    Nombre = ListadeClientes.ElementAt(iNombre++).RowKey,
                    Domicilio = ListadeClientes.ElementAt(iDomicilio++).Domicilio,
                    Edad = ListadeClientes.ElementAt(iEdad++).Edad,
                    Saldo = ListadeClientes.ElementAt(iSaldo++).Saldo,
                    Longitud = ListadeClientes.ElementAt(iLongitud++).Longitud,
                    Latitud = ListadeClientes.ElementAt(iLatitud++).Latitud,
                    Imagen = ListadeClientes.ElementAt(iImagen++).Imagen,
                    ImagenFondo = ListadeClientes.ElementAt(iImagenFondo++).ImagenFondo,
                    Ocupación = ListadeClientes.ElementAt(iOcupacion++).Ocupación,
                    Escuela = ListadeClientes.ElementAt(iEscuela++).Escuela,
                }
                ).ToList();
                int contadorimagen = 0;
                while(contadorimagen < ListadeClientes.Count)
                {
                    elementoimagen = ListadeClientes.ElementAt(contadorimagen).Imagen;
                    elementoimagenfondo = ListadeClientes.ElementAt(contadorimagen).ImagenFondo;
                    var ImagenBlob = Contenedor.GetBlockBlobReference(elementoimagen);
                    var ImagenFondoBlob = Contenedor.GetBlockBlobReference(elementoimagenfondo);
                    var rutaImagen = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    var rutaImagenFondo = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    var ArchivoImagen = System.IO.Path.Combine(rutaImagen, elementoimagen);
                    var ArchivoImagenFondo = System.IO.Path.Combine(rutaImagenFondo, elementoimagenfondo);
                    var StreamImagen = File.OpenWrite(ArchivoImagen);
                    var StreamImagenFondo = File.OpenWrite(ArchivoImagenFondo);
                    await ImagenBlob.DownloadToStreamAsync(StreamImagen);
                    await ImagenFondoBlob.DownloadToStreamAsync(StreamImagenFondo);
                    contadorimagen++;
                }
                Toast.MakeText(this, "Lista Cargada Correctamente", ToastLength.Long).Show();
                listado.Adapter = new DataAdapter(this, ElementosTabla);
                listado.ItemClick += OnListItemClick;
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                throw;
            }
        }

        public void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var DataSend = ElementosTabla[e.Position];
            var DataIntent = new Intent(this, typeof(DataDetailActivity));
            DataIntent.PutExtra("correo", DataSend.Correo);
            DataIntent.PutExtra("imagen", DataSend.Imagen);
            DataIntent.PutExtra("imagenfondo", DataSend.ImagenFondo);
            DataIntent.PutExtra("nombre", DataSend.Nombre);
            DataIntent.PutExtra("edad", DataSend.Edad.ToString());
            //DataIntent.PutExtra("saldo", DataSend.Saldo.ToString());
            DataIntent.PutExtra("domicilio", DataSend.Domicilio);
            DataIntent.PutExtra("latitud", DataSend.Latitud.ToString());
            DataIntent.PutExtra("longitud", DataSend.Longitud.ToString());
            DataIntent.PutExtra("Ocupación", DataSend.Ocupación.ToString());
            DataIntent.PutExtra("Escuela", DataSend.Escuela.ToString());
            StartActivity(DataIntent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class ElementosdelaTabla
    {
        public string Nombre { get; set; }
        public string Domicilio { get; set; }
        public int Edad { get; set; }
        public string ImagenFondo { get; set; }
        public string Imagen { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Correo { get; set; }
        public double Saldo { get; set; }
        public string Ocupación { get; set; }
        public string Escuela { get; set; }
    }

    public class Clientes : TableEntity
    {
        public Clientes(string Categoria, string Nombre)
        {
            PartitionKey = Categoria;
            RowKey = Nombre;
        }
        public Clientes() { }
        public string Domicilio { get; set; }
        public int Edad { get; set; }
        public string ImagenFondo { get; set; }
        public string Imagen { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Correo { get; set; }
        public double Saldo { get; set; }
        public string Ocupación { get; set; }
        public string Escuela { get; set; }
    }
}