using Android.Views;
using Android.Widget;
using Android.App;
using Android.Graphics;
using System.Collections.Generic;

namespace AzureListaClientes
{
    public class DataAdapter : BaseAdapter<ElementosdelaTabla>
    {
        List<ElementosdelaTabla> items;
        Activity context;

        public DataAdapter(Activity context, List<ElementosdelaTabla> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override ElementosdelaTabla this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            view = context.LayoutInflater.Inflate(Resource.Layout.datarow, null);
            view.FindViewById<TextView>(Resource.Id.txtsaldo).Text = item.Ocupación;
            var txtHead = view.FindViewById<TextView>(Resource.Id.txtnombre);
            txtHead.Text = item.Nombre;
            var typeface = Typeface.CreateFromAsset(context.Assets, "Fonts/OdibeeSans-Regular.ttf");
            txtHead.SetTypeface(typeface, TypefaceStyle.Normal);
            var path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), item.Imagen);
            var pathback = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), item.ImagenFondo);
            var Image = BitmapFactory.DecodeFile(path);
            var ImageBack = BitmapFactory.DecodeFile(pathback);
            Image = ResizeBitmap(Image, 100, 100);
            view.FindViewById<ImageView>(Resource.Id.imagen).SetImageDrawable(getRoundedCornerImage(Image, 5));
            view.FindViewById<ImageView>(Resource.Id.imagenfondo).SetImageDrawable(getRoundedCornerImage(ImageBack, 2));
            return view;
        }
        
        public static Android.Support.V4.Graphics.Drawable.RoundedBitmapDrawable getRoundedCornerImage(Bitmap image, int cornerRadius)
        {
            var corner = Android.Support.V4.Graphics.Drawable.RoundedBitmapDrawableFactory.Create(null, image);
            corner.CornerRadius = cornerRadius;
            return corner;
        }

        private Bitmap ResizeBitmap(Bitmap imagenoriginal, int widthmargenoriginal, int heightimagenoriginal)
        {
            Bitmap resizedImage = Bitmap.CreateBitmap(widthmargenoriginal, heightimagenoriginal, Bitmap.Config.Argb8888);
            float Width = imagenoriginal.Width;
            float Height = imagenoriginal.Height;
            var canvas = new Canvas(resizedImage);
            var scala = widthmargenoriginal / Width;
            var xTranslation = 0.0f;
            var yTranslation = (heightimagenoriginal - Height * scala) / 2.0f;
            var Transformacion = new Matrix();
            Transformacion.PostTranslate(xTranslation, yTranslation);
            Transformacion.PreScale(scala, scala);
            var paint = new Paint();
            paint.FilterBitmap = true;
            canvas.DrawBitmap(imagenoriginal, Transformacion, paint);
            return resizedImage;
        }
    }
}