

namespace Examen3Parcial.Views;

public partial class Agregar : ContentPage
{
    //varibles
   
    string base64Image;
    DateTime selectedDateTime;
    Helper.FirebaseHelper firebasehelper = new Helper.FirebaseHelper();
    string fechaingreso;

   


    public Agregar()
	{
		InitializeComponent();
        timePicker.Time = DateTime.Now.TimeOfDay;
        selectedDateTime = datePicker.Date.Add(timePicker.Time);
        resulEntry.Text = $"{selectedDateTime}";
        fechaingreso = $"{selectedDateTime}";

    }

    //botn 
    public async void btnagregar_Clicked(object sender, EventArgs e)
    {
        
        string des = descripcion.Text;
        string fecha = resulEntry.Text;

        if (string.IsNullOrWhiteSpace(des) || base64Image == null || string.IsNullOrWhiteSpace(fecha))
        {
            DisplayAlert("Aviso", "Llene todos los datos para poder ingresar", "OK");
            //return; // No continuar si el campo está vacío
        }
        else
        {
            agregar();
            
            limpiar();


        }
    }

    // comienzo de los botones
    private void btnsalir_Clicked(object sender, EventArgs e)
    {

    }

    private async void btnlista_Clicked(object sender, EventArgs e)
    {
        //var pagina = new Views.ListaLugares();
        //await Navigation.PushAsync(pagina);
    }

    private void btnimagen_Clicked(object sender, EventArgs e)
    {
        TakePhoto();
    }
    //fin de botones

   

    private async Task<string> ConvertImageToBase64(string imagePath)
    {
        byte[] imageBytes;
        using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
        {
            using (MemoryStream ms = new MemoryStream())
            {
                await fs.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }
        }

        // Convertir los bytes de la imagen a una cadena base64
        return Convert.ToBase64String(imageBytes);
    }

    void limpiar()
    {
       // GetCurrentLocation();
        descripcion.Text = "";
        selectedDateTime = datePicker.Date.Add(timePicker.Time);
        fechaingreso = $"{selectedDateTime}";

        capturedImage.Source = null;
        base64Image = null;

    }

    public async void TakePhoto()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                // Guardar el archivo en almacenamiento local
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                // Usar un bloque using para asegurarse de que los streams se cierran correctamente
                using (System.IO.Stream sourceStream = await photo.OpenReadAsync())
                {
                    using (FileStream localFileStream = File.OpenWrite(localFilePath))
                    {
                        await sourceStream.CopyToAsync(localFileStream);
                    }
                }

                // Mostrar la imagen capturada
                capturedImage.Source = ImageSource.FromFile(localFilePath);

                // Convertir la imagen a base64 (asegurarse de que los flujos anteriores están cerrados)
                base64Image = await ConvertImageToBase64(localFilePath);
                //await DisplayAlert("Imagen en Base64", base64Image, "OK");
            }
        }
    }

    //fechas

    private void OnValidateDateTimeClicked(object sender, EventArgs e)
    {
        fechahora();
    }

    void fechahora()
    {
        // Combinar fecha y hora seleccionadas
        DateTime selectedDate = datePicker.Date;
        TimeSpan selectedTime = timePicker.Time;
        selectedDateTime = selectedDate.Add(selectedTime);

        // Validar que la fecha y hora seleccionada no sea anterior a ahora
        if (selectedDateTime < DateTime.Now)
        {
            resultLabel.Text = "La fecha y hora seleccionada no puede ser anterior a la actual.";
            resultLabel.TextColor = Colors.Red;
        }
        else
        {
            resulEntry.Text = $"{selectedDateTime}";
            resultLabel.TextColor = Colors.Green;
        }
    }

    async void agregar()
    {

        //OnUploadImage();
        // UploadFileAsync(_selectedImageStream, "asas");

        
       

        var Notas = new Modelos.Notas
        {
            DESCRIPCION = descripcion.Text,
            FECHA = resulEntry.Text,
            PHOTO_RECORD = base64Image,
            FECHAINGRESO = fechaingreso,

        };

        await firebasehelper.AddProducto(Notas);
        await DisplayAlert("Exito", "Datos Agregados correctamente", "OK");
        //await DisplayAlert("Exito", "Producto Agregado correctamente", "OK");
        //await Navigation.PopAsync();
    }
   

}
