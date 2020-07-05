<template>
    <div id="app">
        <div class="dev-work" v-if="env === 'development'">
            👷 ({{env}}) Last Update: {{ new Date().toTimeString() }} 🔧
            <div>
                <button @click="newFollower('Test')">Follow</button>
                <button @click="newSubscriber({username:'Bob Test', streakMonths: 5, totalMonths: 5})">Sub</button>
            </div>
        </div>
        <div id="main">
            <div v-if="component">
                <component :is="component" :payload="payload"></component>
            </div>
        </div>
    </div>
</template>

<script>
    import {HubConnectionBuilder, LogLevel} from '@microsoft/signalr'
    import FollowerAlert from "./components/alerts/follower-alert";
    import SubAlert from "./components/alerts/sub-alert";

    export default {
        name: 'App',
        components: {SubAlert, FollowerAlert},
        data: () => ({
            connection: null,
            alert: {
                queue: [],
                active: false,
            },
            component: null,
            payload: null,
            env: process.env.NODE_ENV
        }),
        created() {
            this.connection = new HubConnectionBuilder()
                .withUrl("/hub/twitch")
                .configureLogging(LogLevel.Information)
                .build();

            this.connection.onclose(async () => {
                await this.start();
            });

            this.connection.on("follow", this.newFollower);
            this.connection.on("sub", this.newSubscriber);

            this.start()
        },
        methods: {
            async start() {
                try {
                    await this.connection.start();
                } catch (err) {
                    console.log(err);
                    setTimeout(() => this.start(), 5000);
                }
            },
            newFollower(username) {
                this.queueComponent(FollowerAlert, {username}, 8000)
            },
            newSubscriber(payload) {
                this.queueComponent(SubAlert, payload, 12000)
            },
            queueComponent(component, payload, timeout) {
                this.alert.queue.push({
                    component, payload, timeout
                })
                console.log(this.alert);

                if (!this.alert.active) {
                    this.popQueue();
                }
            },
            popQueue() {
                this.alert.active = true;

                const {component, payload, timeout} = this.alert.queue.splice(0, 1)[0]

                this.payload = payload
                this.component = component

                setTimeout(() => {
                    if (this.alert.queue.length > 0) {
                        this.popQueue()
                    } else {
                        this.alert.active = false
                        this.component = null
                        this.payload = null
                    }
                }, timeout)
            }
        },
    }

</script>

<style>
    .dev-work {
        position: absolute;
        top: 4rem;
        right: 4rem;
        color: red;
        font-size: 24px;
        font-weight: bold;
        z-index: 100;
    }

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

</style>
