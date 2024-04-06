export async function getHomePageArticles({ request }) {
    let q = null;

    if (request.url) {
        const url = new URL(request.url);
        q = url.searchParams.get("q");
    }

    const response = q 
        ? await fetch(`${import.meta.env.VITE_FRONTEND_SERVER_BASE_ADDRESS}/search?q=${q}`)
        : await fetch(`${import.meta.env.VITE_FRONTEND_SERVER_BASE_ADDRESS}/articles`);

    const articles = await response.json();
    return { articles, q };
}

export async function searchArticles({ request }) {
    const articles = [];

    if (!request.url) {
        return { articles, q };
    }

    const url = new URL(request.url || '/');
    const q = url.searchParams.get("q");

    return { articles, q };
}