

namespace Examen3Parcial.Views;

public partial class EditNota : ContentPage
{
    private Helper.FirebaseHelper firebaseHelper = new Helper.FirebaseHelper();
    private Modelos.Notas notas;
    DateTime selectedDateTime;
    string base64Image;
    public EditNota(Modelos.Notas notas)
	{
		InitializeComponent();
        timePicker.Time = DateTime.Now.TimeOfDay;
        selectedDateTime = datePicker.Date.Add(timePicker.Time);
        resulEntry.Text = $"{selectedDateTime}";

        cargardatos(notas);


    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        update();
    }

    void cargardatos(Modelos.Notas notas)
    {
        this.notas = notas;

        Descripciontxt.Text = notas.DESCRIPCION;
        base64Image = notas.PHOTO_RECORD;
    }

    private void btnimagen_Clicked(object sender, EventArgs e)
    {
        TakePhoto();
    }


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
                using (Stream sourceStream = await photo.OpenReadAsync())
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
    private async void update()
    {
        notas.DESCRIPCION = Descripciontxt.Text;
        notas.FECHA = resulEntry.Text;
        notas.PHOTO_RECORD = base64Image;

        await firebaseHelper.UpdateProducto(notas.ID_NOTA, notas);
        await DisplayAlert("Exito", "Cargado con Extito", "OK");
        await Navigation.PopAsync();
    }

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
            
        }
    }


}