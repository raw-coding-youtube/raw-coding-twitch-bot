<template>
    <div class="card follow">
        <div class="gif follow"></div>
        <span class="text">{{payload.displayName}}</span>
        <span class="text">Followed</span>
    </div>
</template>

<script>
    export default {
        name: "follower-alert",
        props: {
            payload: {
                type: Object,
                required: true,
            }
        }
    }
</script>

<style scoped>
    .card.follow {
        background-image: url("https://aw-test-bucket.eu-central-1.linodeobjects.com/follow");
    }

    .gif.follow {
        background-image: url("https://aw-test-bucket.eu-central-1.linodeobjects.com/bk");
    }
</style>