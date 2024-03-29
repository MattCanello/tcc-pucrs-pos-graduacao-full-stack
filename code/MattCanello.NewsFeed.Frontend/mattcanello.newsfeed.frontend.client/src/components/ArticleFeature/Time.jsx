import React from 'react';

function Time({ dateTimeString }) {
    const date = new Date(dateTimeString);
    const formatOptions = { day: "2-digit", year: "numeric", month: "2-digit", hour: "2-digit", minute: "2-digit", second: "2-digit", hour12: false };
    const localDateString = date.toLocaleDateString("pt-BR", formatOptions);

    return (
        <time title={localDateString} dateTime={date.toISOString()}>1 minuto atrás</time>
  );
}

export default Time;