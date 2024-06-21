import { isServiceOnline } from './Home';

export async function getChannelArticles({ params }) {
    if (!isServiceOnline()) {
        return [];
    }

    const response = await fetch(`${import.meta.env.VITE_FRONTEND_SERVER_BASE_ADDRESS}/articles/channel/${params.channelId}`);
    const data = await response.json();
    return data;
}

export async function getChannelList() {
    if (!isServiceOnline()) {
        return [];
    }

    const response = await fetch(`${import.meta.env.VITE_FRONTEND_SERVER_BASE_ADDRESS}/channels`);
    const data = await response.json();
    return data;
}