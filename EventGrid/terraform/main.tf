terraform {
  required_version = ">= 1.3.4"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">= 4.40.0"
    }
  }
}

provider "azurerm" {
  features {}

  subscription_id = var.subscription_id
}

resource "azurerm_resource_group" "example" {
  name     = "eventgrid-rg"
  location = var.location
}

resource "azurerm_eventgrid_topic" "name" {
  name = "mynamespacetopic"
  location = azurerm_resource_group.example.location
  resource_group_name = azurerm_resource_group.example.name

  input_schema = "EventGridSchema"
  public_network_access_enabled = true
}

resource "azurerm_storage_account" "example" {
  name                     = "egdevstorageacc"
  resource_group_name      = azurerm_resource_group.example.name
  location                 = azurerm_resource_group.example.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_queue" "example" {
  name               = "mysamplequeue"
  storage_account_id = azurerm_storage_account.example.id
}

resource "azurerm_eventgrid_event_subscription" "example" {
  name              = "storage-queue-subscription"
  scope             = azurerm_eventgrid_topic.name.id
  event_delivery_schema = "EventGridSchema"

  storage_queue_endpoint {
    storage_account_id = azurerm_storage_account.example.id
    queue_name         = azurerm_storage_queue.example.name
  }
}