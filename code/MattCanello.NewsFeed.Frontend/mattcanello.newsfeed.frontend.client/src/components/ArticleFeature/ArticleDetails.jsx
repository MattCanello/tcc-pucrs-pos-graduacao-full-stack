import React from 'react';
import '../../style/ArticleDetails.css';

function ArticleDetails({ summary, description }) {
    let count = 0;
    const paragraphs = description.map(line => <p key={count++}>{line}</p>);

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