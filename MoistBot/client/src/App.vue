<template>
    <div id="app">
        <div id="main">
<!--            <div class="card">-->
<!--                <div class="gif"></div>-->
<!--                <span>TY</span>-->
<!--                <span>Followed</span>-->
<!--            </div>-->
        </div>
    </div>
</template>

<script>

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/hub/twitch")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    async function start() {
        try {
            await connection.start();
        } catch (err) {
            console.log(err);
            setTimeout(() => start(), 5000);
        }
    }

    connection.onclose(async () => {
        await start();
    });

    connection.on("follow", (username) => {
        const div = document.createElement("div");
        const gif = document.createElement("div");
        gif.classList.add('gif', "follow")
        const user = document.createElement("span")
        user.classList.add('text')
        user.textContent = username;
        const follow = document.createElement("span")
        follow.classList.add('text')
        follow.textContent = "Followed!";

        div.classList.add("card", "follow")
        div.append(user)
        div.append(follow)
        div.append(gif)

        document.getElementById("main").appendChild(div);

        setTimeout(function () {
            div.remove();
        }, 8000)
    });

    connection.on("sub", (payload) => {
        console.log(payload)
        const div = document.createElement("div");
        const gif = document.createElement("div");
        gif.classList.add('gif', "sub")
        const user = document.createElement("span")
        user.textContent = payload.username;
        user.classList.add('text')
        const sub = document.createElement("span")
        sub.classList.add('text')
        const total = document.createElement("span")
        total.classList.add('text')

        if (payload.total === 0) {
            sub.textContent = "New Subscriber!";
        } else {
            sub.textContent = "Subscribed AGAIN!";
            total.textContent = payload.total + " Months!";
        }

        div.classList.add("card", "sub")
        div.append(user)
        div.append(sub)
        div.append(gif)
        if (payload.total > 0) {
            div.append(total)
        }

        document.getElementById("main").appendChild(div);

        setTimeout(function () {
            div.remove();
        }, 12000)
    });

    start();

    export default {
        name: 'App',
    }
</script>

<style>
    div#main {
        display: flex;
        position: fixed;
        width: 100%;
        height: 100%;
        margin-top: 5px;
        justify-content: center;
    }

    div.card {
        display: flex;
        position: relative;
        justify-content: flex-end;
        align-items: center;
        flex-direction: column;
        width: 230px;
        height: 265px;

        background-size: cover;
        border-radius: 5px;
        box-shadow: 0 0 10px 1px hotpink;
    }

    div.card.follow {
        background-image: url("https://aw-test-bucket.eu-central-1.linodeobjects.com/follow");
    }

    div.card.sub {
        background-image: url("https://aw-test-bucket.eu-central-1.linodeobjects.com/follow");
    }

    div.card > .text {
        z-index: 10;
        font-size: 20px;
        font-weight: bold;
        font-family: "Comic Sans MS";
        color: deeppink;
        text-shadow: 0 0 4px black;
    }

    .gif {
        position: absolute;
        left: 0;
        background-size: cover;
        width: 100%;
        height: 100%;
        opacity: 0.2;
    }

    .gif.follow {
        background-image: url("https://aw-test-bucket.eu-central-1.linodeobjects.com/bk");
    }

    .gif.sub {
        background-image: url("https://aw-test-bucket.eu-central-1.linodeobjects.com/sub");
    }
</style>
