<template>
    <div class="card sub">
        <div class="gif sub"></div>
        <span class="text">{{username}} x{{total}}</span>
        <span class="text">{{streak}}</span>
    </div>
</template>

<script>
    export default {
        name: "sub-alert",
        props: {
            payload: {
                type: Object,
                required: true,
            }
        },
        computed: {
            username() {return this.payload.username},
            total() {return this.payload.totalMonths},
            streak() {
                const streak =this.payload.streakMonths;
                if(streak === 2) return "Double Wammy!"
                if(streak === 3) return "Triple Granny!"
                if(streak === 4) return "🍀 Lucky Lad 🍀"
                if(streak === 5) return "✋ High Five Bro ✋"
                if(streak === 6) return "🦄 Unstoppable! 🦄"
                if(streak === 7) return "🌈 Pot a gold 🌈"
                if(streak === 8) return "🔂 It's a loopy 🔂"
                if(streak === 9) return "☁ Cloudy up here ☁"
                if(streak > 10) return "🔥🔥🔥🔥🔥🔥🔥🔥🔥🔥"
                return "";
            },
        }
    }
</script>

<style scoped>
    div.card.sub {
        background-image: url("https://aw-test-bucket.eu-central-1.linodeobjects.com/follow");
    }

    .gif.sub {
        background-image: url("https://aw-test-bucket.eu-central-1.linodeobjects.com/sub");
    }
</style>