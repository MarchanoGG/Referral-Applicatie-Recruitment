<script>
import Modal from './Modal.vue';
export default {
    data() {
        return {
            show: true,
            dataTarget: this.prop.dataTarget
        };
    },
    prop: {
        dataTarget: String
    },
    components: { Modal }
}
</script>
<template>
    <button type="button" class="navbar-brand btn btn-primary" data-bs-toggle="modal" data-bs-target="dataTarget">
        <i class="bi bi-plus"></i>
    </button>
    <!-- Modal -->
    <Modal/>
</template>

<style>

</style>