<script setup lang="ts">
const lorem =
  "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
</script>

<template>
  <q-layout view="hHh lpR fFf">
    <q-page-container>
      <!-- <router-view /> -->
      <q-card dark bordered class="bg-grey-9 my-card">
        <q-card-section>
          <div class="text-h6">Our Changing Planet</div>
          <div class="text-subtitle2">by John Doe</div>
        </q-card-section>

        <q-separator dark inset />

        <q-card-section>
          {{ lorem }}
        </q-card-section>
      </q-card>
    </q-page-container>
  </q-layout>
</template>

<style lang="sass" scoped>
.my-card
  width: 100%
  max-width: 250px
</style>
