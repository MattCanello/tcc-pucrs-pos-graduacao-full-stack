export async function getChannelArticles({ params }) {
    const response = await fetch(`${import.meta.env.VITE_FRONTEND_SERVER_BASE_ADDRESS}/articles/channel/${params.channelId}`);
    const data = await response.json();
    return data;
}