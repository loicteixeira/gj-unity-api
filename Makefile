.PHONY: help doc_generate doc_publish doc_serve
.DEFAULT_GOAL := help

help: ## See what commands are available.
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36mmake %-15s\033[0m # %s\n", $$1, $$2}'

doc_generate: ## Build documentation. Make sure `doxygen` is in the $PATH (e.g. `ln -s /Applications/Doxygen.app/Contents/Resources/doxygen /usr/local/bin/doxygen`).
	pushd ./Documentation/; doxygen ./Doxyfile; popd;

doc_publish: ## Publish the generated documentation to `gh-pages`.
	git push origin `git subtree split --prefix Documentation/Output/html master`:gh-pages --force

doc_serve: ## Serve the generated documentation on `http://localhost:8000`
	pushd ./Documentation/Output/html; python2 -m SimpleHTTPServer 8000; popd;
