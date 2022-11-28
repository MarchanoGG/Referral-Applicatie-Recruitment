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
            <h1>Users management - Recruiters Only</h1>
            <nav class="navbar navbar-expand-lg bg">
                <ButtonModal :dataTarget="test" />
                <!-- <button class="navbar-brand btn btn-primary"><i class="bi bi-plus"></i></button> -->
            </nav>
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">Name</th>
                        <th scope="col">Surname</th>
                        <th scope="col">Points</th>
                        <th scope="col">Settings</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <th scope="row">1</th>
                        <td>Mark</td>
                        <td>Otto</td>
                        <td>50</td>
                        <td>
                            <a class=""><ins>Edit</ins></a>&nbsp;
                            <a class=""><ins>Delete</ins></a>
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">2</th>
                        <td>Jacob</td>
                        <td>Thornton</td>
                        <td>30</td>
                        <td>
                            <a class=""><ins>Edit</ins></a>&nbsp;
                            <a class=""><ins>Delete</ins></a>
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">3</th>
                        <td>Larry</td>
                        <td>the Bird</td>
                        <td>20</td>
                        <td>
                            <a class=""><ins>Edit</ins></a>&nbsp;
                            <a class=""><ins>Delete</ins></a>
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">4</th>
                        <td>Mike</td>
                        <td>the Dog</td>
                        <td>50</td>
                        <td>
                            <a class=""><ins>Edit</ins></a>&nbsp;
                            <a class=""><ins>Delete</ins></a>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<style>

</style>