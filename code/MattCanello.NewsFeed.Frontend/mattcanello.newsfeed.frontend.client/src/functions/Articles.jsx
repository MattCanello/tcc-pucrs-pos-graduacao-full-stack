export async function getArticle({ params }) {
    const response = await fetch(`${import.meta.env.VITE_FRONTEND_SERVER_BASE_ADDRESS}/articles/${params.feedId}/${params.articleId}`);
    const data = await response.json();
    return data;
}