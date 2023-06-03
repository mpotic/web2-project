export function formatDateTime(dateTime) {
  const date = new Date(dateTime);
  const options = { day: 'numeric', month: 'long', year: 'numeric' };
  return date.toLocaleDateString(undefined, options);
}
