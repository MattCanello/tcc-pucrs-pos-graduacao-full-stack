export async function getArticle({ params }) {
    const response = await fetch(`/articles/${params.feedId}/${params.articleId}`);
    const data = await response.json();
    return data;
}