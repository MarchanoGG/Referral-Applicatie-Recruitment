<script>
export default {
    data() {
        return {
            show: true
        }
    },
    prop: {
        
    }
    // methods: 
}
</script>
<template>
    <!-- <button class="btn btn-success" data-bs-toggle="modal" data-bs-target="#exampleModal">
        Open {{show}}
    </button> -->
    <!-- <div v-if="show" class="modal">hello form</div> -->
    <!-- Button trigger modal -->
    <button type="button" class="navbar-brand btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
        <i class="bi bi-plus"></i>
    </button>
</template>

<style>

</style>