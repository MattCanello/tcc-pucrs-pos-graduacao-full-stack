import React from 'react';
import '../../style/ArticleList.css';
import Article from './Article';

function ArticleList() {
    return (
        <section>
            <Article
                title="The pet I'll never forget: Mrs Hinch on her alpacas, 'who fill my gardin with happiness'"
                channelName="The Guardian"
                publishDate="2023-12-28 14:03:00"
                imageTitle="'They're so fluffy!' (From left) Rodney, Roy and Raymond."
                imageSrc="https://i.guim.co.uk/img/media/4b91e61aa1e89de5f2128166a6d837d4e07ac470/0_252_1440_864/master/1440.jpg?width=1300&dpr=2&s=none"
                summary="We chose Raymond, Row and Rodney as babies. They are gentel, curious personalities - who sometimes strut into the house in pursuit of spare rains..."
                description={[
                    "I loved alpacas when I was a child, but never did I think I would own three. I love their little faces and the fact that they are big, but so gentle.They are very shy and not very confident.",
                    "We moved house last year and our new home has a bit of land. I had always wanted to have more animals; I read up on everything about alpacas for so long that it came to the point where I was like: “Right, I'm going to go for it. We could give them a wonderful home.” And we have."
                ]}
                authors="Emine Saner"
                />

            <Article
                title="2024 terá poucos 'feriadões' prolongados; veja as folgas previstas"
                channelName="G1"
                publishDate="2023-11-21 04:02:00"
                imageTitle="'Feriadôes' de 2024: veja as folgas previstas"
                imageSrc="https://s2-g1.glbimg.com/wNL5K8Sa6WSocIECL0Q7D63WS5w=/1200x/smart/filters:cover():strip_icc()/s02.video.glbimg.com/x720/12131145.jpg"
                summary="Próximo ano terá dez feriados nacionais, sendo que quatro serão no fim de semana."
                description={[
                    "O ano de 2024 terá poucos feriados prolongados. Serão dez feriados nacionais, dos quais apenas três vão ser vizinhos de um fim de semana, isto é, caem numa segunda ou na sexta-feira.",
                    "São eles: o da Confraternização Universal, em 1° de janeiro; o da Paixão de Cristo, em 29 de março; e o da Proclamação da República, no dia 15 de novembro (veja a lista com todos os feriados ao final da reportagem)."
                ]}
                authors="Anaísa Cattucci"
                />
        </section>
    );
}

export default ArticleList;