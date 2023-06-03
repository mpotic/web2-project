const urlToFile = async (imageUrl) => {
  try {
    const response = await fetch(imageUrl);
    const blob = await response.blob();

    // Extract file extension from image URL
    const fileExtension = imageUrl.split('.').pop();
    const fileName = `image.${fileExtension}`;

    const file = new File([blob], fileName, { type: blob.type });
    console.log(file);
  } catch (error) {
    console.error(error);
  }
};
