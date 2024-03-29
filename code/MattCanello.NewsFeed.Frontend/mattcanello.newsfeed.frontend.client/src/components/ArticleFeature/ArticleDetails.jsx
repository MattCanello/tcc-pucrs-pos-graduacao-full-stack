import React from 'react';
import '../../style/ArticleDetails.css';

function ArticleDetails({ summary, description }) {
    const paragraphs = description.map(line => <p>{line}</p>);

    return (
        <details>
            <summary>
                {summary}
            </summary>

            {paragraphs}
        </details>
    );
}

export default ArticleDetails;