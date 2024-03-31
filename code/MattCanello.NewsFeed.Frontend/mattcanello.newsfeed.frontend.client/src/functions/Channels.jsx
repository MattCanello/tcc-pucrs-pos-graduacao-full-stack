export async function getChannelArticles({ params }) {
    const response = await fetch(`/articles/channel/${params.channelId}`);
    const data = await response.json();
    return data;
}