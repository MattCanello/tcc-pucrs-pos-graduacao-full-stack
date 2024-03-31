import React from 'react';
import '../../style/ArticleDetails.css';

function ArticleDetails({ summary, description, expanded }) {
    let count = 0;
    const paragraphs = description.map(line => <p key={count++}>{line}</p>);

    const summaryData = description.length == 0
        ? <p>{summary}</p>
        : <summary>{summary}</summary>

    const details = expanded
        ? <details open>{summaryData}{paragraphs}</details>
        : <details>{summaryData}{paragraphs}</details>;

    return description.length == 0
        ? summaryData
        : details;
}

export default ArticleDetails;