package devopsprojectstack

import (
	sdk "github.com/pulumi/pulumi-azure-native-sdk/v2"
	ad "github.com/pulumi/pulumi-azuread/sdk/v6/go/azuread"

	keyvault "github.com/pulumi/pulumi-azure/sdk/v6/go/azure/keyvault"
	devops "github.com/pulumi/pulumi-azuredevops/sdk/v3/go/azuredevops"
	"github.com/pulumi/pulumi/sdk/v3/go/pulumi"
	"github.com/pulumi/pulumi/sdk/v3/go/pulumi/config"
)

type DevopsOutputs struct {
	ProjectName              pulumi.StringOutput
	ServicePrincipalClientId pulumi.StringOutput
	ServicePrincipalTenant   pulumi.StringOutput
	ServicePrincipalSecret   pulumi.StringOutput
	Secret                   pulumi.StringOutput
	ServiceEndpoint          pulumi.StringOutput
	PipelineId               pulumi.StringOutput
}

func DevopsStack(ctx *pulumi.Context, providerLocation *sdk.Provider, resourceGroupName pulumi.StringOutput) (DevopsOutputs, error) {

	devopsConfig := config.New(ctx, "devopsConfig")
	githubConfig := config.New(ctx, "github")

	current, err := ad.GetClientConfig(ctx, pulumi.CompositeInvoke(), nil)
	if err != nil {
		return DevopsOutputs{}, err
	}

	aoc2023App, err := ad.NewApplication(ctx, devopsConfig.Require("appName"), &ad.ApplicationArgs{
		DisplayName: pulumi.String(devopsConfig.Require("appName")),
		Owners: pulumi.StringArray{
			pulumi.String(current.ObjectId),
		},
	})
	if err != nil {
		return DevopsOutputs{}, err
	}
	principal, err := ad.NewServicePrincipal(ctx, "service-principal", &ad.ServicePrincipalArgs{
		ClientId:                  aoc2023App.ClientId,
		AppRoleAssignmentRequired: pulumi.Bool(false),
		Owners: pulumi.StringArray{
			pulumi.String(current.ObjectId),
		},
	})
	if err != nil {
		return DevopsOutputs{}, err
	}

	password, err := ad.NewServicePrincipalPassword(ctx, "service-principal-pwd", &ad.ServicePrincipalPasswordArgs{
		ServicePrincipalId: principal.ID(),
	})
	if err != nil {
		return DevopsOutputs{}, err
	}

	vault, err := keyvault.NewKeyVault(ctx, "aoc2023-vault", &keyvault.KeyVaultArgs{
		SkuName:                  pulumi.String("standard"),
		EnabledForDiskEncryption: pulumi.Bool(true),
		EnableRbacAuthorization:  pulumi.Bool(false),
		TenantId:                 pulumi.String(config.Require(ctx, "tenantId")),
		AccessPolicies: keyvault.KeyVaultAccessPolicyArray{
			&keyvault.KeyVaultAccessPolicyArgs{
				ObjectId: pulumi.String(config.Require(ctx, "userObjectId")),
				TenantId: pulumi.String(config.Require(ctx, "tenantId")),
				KeyPermissions: pulumi.StringArray{
					pulumi.String("Create"),
					pulumi.String("Delete"),
					pulumi.String("Get"),
					pulumi.String("List"),
					pulumi.String("Encrypt"),
					pulumi.String("Decrypt"),
					pulumi.String("Purge"),
					pulumi.String("Recover"),
					pulumi.String("Update"),
					pulumi.String("GetRotationPolicy"),
					pulumi.String("SetRotationPolicy"),
				},
				SecretPermissions: pulumi.StringArray{
					pulumi.String("Set"),
					pulumi.String("Delete"),
					pulumi.String("Get"),
					pulumi.String("List"),
					pulumi.String("Purge"),
					pulumi.String("Recover"),
					pulumi.String("Backup"),
				},
			},
			&keyvault.KeyVaultAccessPolicyArgs{
				ObjectId:      aoc2023App.ClientId,
				ApplicationId: aoc2023App.ClientId,
				TenantId:      pulumi.String(config.Require(ctx, "tenantId")),
				SecretPermissions: pulumi.StringArray{
					pulumi.String("Set"),
					pulumi.String("Delete"),
					pulumi.String("Get"),
					pulumi.String("List"),
					pulumi.String("Purge"),
					pulumi.String("Recover"),
					pulumi.String("Backup"),
				},
			},
		},
		ResourceGroupName: resourceGroupName,
	}, pulumi.Provider(providerLocation))
	if err != nil {
		return DevopsOutputs{}, err
	}

	secret, err := keyvault.NewSecret(ctx, "devops-sp-secret", &keyvault.SecretArgs{
		KeyVaultId: vault.ID(),
		Value:      password.Value,
	})
	if err != nil {
		return DevopsOutputs{}, err
	}

	project, err := devops.NewProject(ctx, "aoc2023", &devops.ProjectArgs{
		Name:             pulumi.String(devopsConfig.Require("projectName")),
		Visibility:       pulumi.String("private"),
		VersionControl:   pulumi.String("Git"),
		WorkItemTemplate: pulumi.String("Agile"),
		Features: pulumi.StringMap{
			"testplans": pulumi.String("enabled"),
			"artifacts": pulumi.String("enabled"),
			"boards":    pulumi.String("enabled"),
			"pipelines": pulumi.String("enabled"),
		},
		Description: pulumi.String("my devops project"),
	}, pulumi.Provider(providerLocation))

	if err != nil {
		return DevopsOutputs{}, err
	}

	// create a github service connection
	serviceEndpoint, err := devops.NewServiceEndpointGitHub(ctx, "github-service-endpoint", &devops.ServiceEndpointGitHubArgs{
		ProjectId:           project.ID(),
		ServiceEndpointName: pulumi.String("Github Connection"),
		AuthPersonal: &devops.ServiceEndpointGitHubAuthPersonalArgs{
			PersonalAccessToken: githubConfig.RequireSecret("token"),
		},
		Description: pulumi.String("Github Connection"),
	})
	if err != nil {
		return DevopsOutputs{}, err
	}

	pipeline, err := devops.NewBuildDefinition(ctx, "aoc2023-pipeline", &devops.BuildDefinitionArgs{
		ProjectId: project.ID(),
		Name:      pulumi.String("AOC2023-CI"),
		Repository: &devops.BuildDefinitionRepositoryArgs{
			RepoType:            pulumi.String("GitHub"),
			RepoId:              pulumi.String("ekrall1/aoc2023"),
			BranchName:          pulumi.String("main"),
			YmlPath:             pulumi.String("azure-pipelines.yml"),
			ServiceConnectionId: serviceEndpoint.ID(),
		},
		CiTrigger: &devops.BuildDefinitionCiTriggerArgs{
			UseYaml: pulumi.Bool(true),
		},
		QueueStatus: pulumi.String("enabled"),
	})
	if err != nil {
		return DevopsOutputs{}, err
	}

	// TODO: put the secret in a vault
	return DevopsOutputs{
		ProjectName:              project.Name,
		Secret:                   password.Value,
		ServicePrincipalClientId: principal.ID().ToStringOutput(),
		ServicePrincipalSecret:   secret.ResourceId,
		ServicePrincipalTenant:   principal.ApplicationTenantId,
		ServiceEndpoint:          serviceEndpoint.Description.Elem().ToStringOutput(),
		PipelineId:               pipeline.ID().ToStringOutput(),
	}, nil

}
