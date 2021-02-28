<template>
    <div id="app">
        <div class="dev-work warn" v-if="env === 'development'">
            👷 ({{env}}) Last Update: {{ new Date().toTimeString() }} 🔧
            <div>
                <button @click="testGet('/api/test/follow')">Follow</button>
                <button @click="testGet(`/api/test/sub?count=${Math.random() * 10 | 0}`)">Sub</button>
            </div>
        </div>
        <div id="main">
            <div v-if="component">
                <component :is="component" :payload="payload"></component>
            </div>
        </div>
        <div class="offline warn" v-if="offline">
            Service Offline
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
            component: null,
            payload: null,
            env: process.env.NODE_ENV,
            version: 0,
            offline: true,
        }),
        watch: {
            version: function (n, o) {
                if (n && o && n !== o) {
                    location.reload()
                }
            }
        },
        created() {
            this.pollVersion();

            const builder = new HubConnectionBuilder()
                .withUrl("/hub/twitch");

            if (this.env === 'development') builder.configureLogging(LogLevel.Information);

            this.connection = builder.build();

            this.connection.onclose(async () => {
                await this.start()
            });

            this.connection.on("HandleEvent", ({target, attributes, displayTime}) => {
                this.component = this.componentFactory(target)
                this.payload = attributes
                setTimeout(() => {
                    this.component = null
                    this.payload = null
                }, displayTime)
            });

            this.start()
        },
        methods: {
            async start() {
                try {
                    await this.connection.start();
                } catch (err) {
                    setTimeout(() => this.start(), 5000);
                }
            },
            pollVersion() {
                fetch('/api/health')
                    .then(res => res.json())
                    .then(version => {
                        this.version = version
                        this.offline = false
                    })
                    .catch(err => {
                        console.log(err)
                        this.offline = true
                    })
                    .finally(() => {
                        setTimeout(this.pollVersion, 10000)
                    })
            },
            componentFactory(target) {
                if (target === 'Follow') return FollowerAlert;
                if (target === 'Subscribe') return SubAlert;
            },
            testGet(route) {
                return fetch(route)
            }
        },
    }

</script>

<style>
    body {
        margin: 0;
    }

    .warn {
        position: absolute;
        color: red;
        font-size: 24px;
        font-weight: bold;
        z-index: 100;
    }

    .offline {
        bottom: 24px;
        right: 166px;
        font-size: 26px;
    }

    .dev-work {
        top: 4rem;
        right: 4rem;
    }

    div#main {
        display: flex;
        position: fixed;
        width: 100%;
        height: 100%;
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
