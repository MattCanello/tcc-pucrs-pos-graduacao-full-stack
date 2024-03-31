export async function getHomePageArticles() {
    const response = await fetch("/articles");
    const data = await response.json();
    return data;
}