export async function getQuerySearchParam(params) {
    let q = null;

    if (params && params.request && params.request.url) {
        const url = new URL(params.request.url);
        q = url.searchParams.get("q");
    }

    return { q };
}

export async function getHomePageArticles() {
    const response = await fetch(`${import.meta.env.VITE_FRONTEND_SERVER_BASE_ADDRESS}/articles`);
    const articles = await response.json();
    return articles;
}

export async function searchArticles(q) {
    const response = q
        ? await fetch(`${import.meta.env.VITE_FRONTEND_SERVER_BASE_ADDRESS}/search?q=${q}`)
        : await fetch(`${import.meta.env.VITE_FRONTEND_SERVER_BASE_ADDRESS}/articles`);

    const articles = await response.json();
    return articles;
}