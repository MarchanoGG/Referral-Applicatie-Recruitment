<script>
import Modal from "../ButtonModal.vue";
import Button from "../Button.vue";
import ButtonModal from "../ButtonModal.vue";
export default{
    data() {
        return {
            users: [
                {
                    id: "1",
                    firstname: "Mark",
                    lastname: "Otto",
                    email: "markotto@gmail.com",
                }
            ],
        };
    },
    components: { Modal, Button, ButtonModal }
}
</script>
<template>
    <div id="users">
        <div class="container-fluid">
            <h1>Users management</h1>
            <nav class="navbar navbar-expand-lg bg">
                <ButtonModal :dataTarget="test" />
                <!-- <button class="navbar-brand btn btn-primary"><i class="bi bi-plus"></i></button> -->
            </nav>
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">First</th>
                        <th scope="col">Last</th>
                        <th scope="col">Email</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <th scope="row">1</th>
                        <td>Mark</td>
                        <td>Otto</td>
                        <td>markotto@gmail.com</td>
                    </tr>
                    <tr>
                        <th scope="row">2</th>
                        <td>Jacob</td>
                        <td>Thornton</td>
                        <td>jacobthornton@gmail.com</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<style>

</style>