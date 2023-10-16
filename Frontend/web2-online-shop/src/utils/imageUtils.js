const urlToFile = async (imageUrl) => {
  try {
    const response = await fetch(imageUrl);
    const blob = await response.blob();

    // Extract file extension from image URL
    const fileExtension = imageUrl.split('.').pop();
    const fileName = `image.${fileExtension}`;

    const file = new File([blob], fileName, { type: blob.type });
  } catch (error) {
    console.error(error);
  }
};

export function imageJsonToImageUrl(imageJson) {
  // Extract the base64 image data from the JSON
  var image_data = imageJson.profileImage;

  // Create a new Image element
  var image = new Image();

  // Set the image source to the base64 data
  image.src = 'data:image/jpeg;base64,' + image_data;

  // Rotate the image by 90 degrees clockwise
  var canvas = document.createElement('canvas');
  var context = canvas.getContext('2d');
  canvas.width = image.height;
  canvas.height = image.width;
  context.rotate((90 * Math.PI) / 180);
  context.drawImage(image, 0, -canvas.width);

  // Resize the image to a width of 300 pixels
  var resizedCanvas = document.createElement('canvas');
  var resizedContext = resizedCanvas.getContext('2d');
  resizedCanvas.width = 300;
  resizedCanvas.height = image.width * (300 / image.height);
  resizedContext.drawImage(
    canvas,
    0,
    0,
    resizedCanvas.width,
    resizedCanvas.height
  );

  // Get the base64 data of the resized image
  var resizedImage = resizedCanvas.toDataURL('image/jpeg');

  return resizedImage;
}
