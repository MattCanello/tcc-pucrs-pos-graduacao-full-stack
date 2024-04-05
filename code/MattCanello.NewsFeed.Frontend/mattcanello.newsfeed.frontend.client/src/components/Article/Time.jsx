import React from 'react';
import { useEffect, useState } from 'react';

function Time({ dateTimeString, useAbsoluteTime }) {
    const [now, setNow] = useState(new Date());
    const [date] = useState(new Date(dateTimeString));

    useEffect(() => {
        if (useAbsoluteTime) {
            return;
        }

        const interval = setInterval(() => setNow(new Date()), 1000);
        return () => {
            clearInterval(interval);
        };
    }, []);

    const formatOptions = { day: "2-digit", year: "numeric", month: "2-digit", hour: "2-digit", minute: "2-digit", second: "2-digit", hour12: false };
    const localDateString = date.toLocaleDateString("pt-BR", formatOptions);
    const absoluteDateString = date.toLocaleString('pt-BR', { dateStyle: "short", timeStyle: "short" });

    function relativeTime(time) {
        var msPerMinute = 60 * 1000;
        var msPerHour = msPerMinute * 60;
        var msPerDay = msPerHour * 24;
        var msPerMonth = msPerDay * 30;
        var msPerYear = msPerDay * 365;

        var elapsed = time - date;

        if (elapsed < msPerMinute) {
            return Math.round(elapsed / 1000) + ' segundos atrás';
        }
        else if (elapsed < msPerHour) {
            return Math.round(elapsed / msPerMinute) + ' minutos atrás';
        }
        else if (elapsed < msPerDay) {
            return Math.round(elapsed / msPerHour) + ' horas atrás';
        }
        else if (elapsed < msPerMonth) {
            return 'aprox. ' + Math.round(elapsed / msPerDay) + ' dias atrás';
        }
        else if (elapsed < msPerYear) {
            return 'aprox. ' + Math.round(elapsed / msPerMonth) + ' meses atrás';
        }
        else {
            return 'aprox. ' + Math.round(elapsed / msPerYear) + ' anos atrás';
        }
    }

    return (
        <time title={localDateString} dateTime={date.toISOString()}>{useAbsoluteTime ? absoluteDateString : relativeTime(now)}</time>
    );
}

export default Time;