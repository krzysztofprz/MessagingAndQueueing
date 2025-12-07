variable "subscription_id" {
  description = "Azure subscription ID"
  type        = string
  sensitive   = false
}

variable "environment" {
  type    = string
  default = "dev"
}

variable "location" {
  type    = string
  default = "polandcentral"
}

variable "tags" {
  type = map(string)
  default = {
    "env"      = "dev"
    "resource" = "eventgrid"
  }
}

